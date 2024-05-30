import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@core/base/base.component';
import { Channel } from '@core/models/channel';
import { NotificationService } from '@core/services/notification.service';
import { SpinnerOverlayService } from '@core/services/spinner-overlay.service';
import { ChannelService } from "@core/services/channel.service";
import { switchMap } from "rxjs";
import { ParsingService } from "@core/services/parsing.service";
import { FormControl, FormGroup, Validators } from "@angular/forms";

@Component({
    selector: 'app-channels-parsing-page',
    templateUrl: './channels-parsing-page.component.html',
    styleUrls: ['./channels-parsing-page.component.sass'],
})
export class ChannelsParsingPageComponent extends BaseComponent implements OnInit {
    public channels: Channel[] = [];

    public parsingForm = new FormGroup(
        {
            parsingDate: new FormControl(
                '',
                [
                    Validators.required
                ],
            ),
        },
        {
            updateOn: 'blur',
        },
    );

    public saveChannelForm = new FormGroup(
        {
            channelLink: new FormControl(
                '',
                [
                    Validators.required,
                    Validators.pattern(/^https:\/\/t\.me\/.+$/)
                ],
            ),
        },
        {
            updateOn: 'blur',
        },
    );

    constructor(
        private channelService: ChannelService,
        private parsingService: ParsingService,
        private spinnerService: SpinnerOverlayService,
        private notifications: NotificationService,
    ) {
        super();
    }

    ngOnInit(): void {
        this.loadChannels();
    }

    private loadChannels() {
        this.spinnerService.show();
        this.channelService.getChannelsToParse()
            .pipe(this.untilThis)
            .subscribe(
                channels => {
                    this.channels = channels;
                    this.spinnerService.hide();
                },
                error => {
                    console.log(error);
                    this.notifications.showErrorMessage('Сталася помилка при завантаженні каналів');
                    this.spinnerService.hide();
                },
            );
    }

    parseChannels() {
        console.log(this.parsingForm.value.parsingDate!)
        const parsingDate = new Date(this.parsingForm.value.parsingDate!);
        if (parsingDate > new Date()) {
            this.notifications.showWarningMessage('Дата повинна бути меншою за поточну');
            return;
        }

        this.notifications.showSuccessMessage('Парсинг було успішно розпочато');

        this.parsingService
            .parseChannels(parsingDate)
            .pipe(this.untilThis)
            .subscribe({
                next: () => this.notifications.showSuccessMessage('Парсинг було повністю виконано та успішно завершено'),
                error: () => this.notifications.showErrorMessage('Трапилася помилка. Парсинг було перевано')
            });
    }

    saveChannel() {
        this.spinnerService.show();

        const channelLink = this.saveChannelForm.value.channelLink!;

        this.channelService
            .saveChannel(channelLink)
            .pipe(switchMap(() => this.channelService.getChannelsToParse()))
            .pipe(this.untilThis)
            .subscribe({
                next: (channels) => {
                    this.channels = channels;
                    this.notifications.showSuccessMessage('Канал успішно збережено');
                    this.spinnerService.hide()
                    this.saveChannelForm.reset();
                },
                error: (error) => {
                    this.notifications.showErrorMessage('Трапилася помилка');
                    console.log(error);
                    this.spinnerService.hide();
                }
            });
    }

    deleteChannel(channelId: number) {
        this.spinnerService.show();
        this.channelService
            .deleteChannel(channelId)
            .pipe(switchMap(() => this.channelService.getChannelsToParse()))
            .pipe(this.untilThis)
            .subscribe({
                next: (channels) => {
                    this.channels = channels;
                    this.notifications.showSuccessMessage('Канал успішно видалено зі списку');
                    this.spinnerService.hide()
                },
                error: (error) => {
                    this.notifications.showErrorMessage('Трапилася помилка');
                    console.log(error);
                    this.spinnerService.hide();
                }
            });
    }
}
