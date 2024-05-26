import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@core/base/base.component';
import { Channel } from '@core/models/channel';
import { NotificationService } from '@core/services/notification.service';
import { SpinnerOverlayService } from '@core/services/spinner-overlay.service';
import { ChannelService } from "@core/services/channel.service";

@Component({
    selector: 'app-channels-parsing-page',
    templateUrl: './channels-parsing-page.component.html',
    styleUrls: ['./channels-parsing-page.component.sass'],
})
export class ChannelsParsingPageComponent extends BaseComponent implements OnInit {
    public channels: Channel[] = [];

    constructor(
        private channelService: ChannelService,
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
                    this.channels = [...this.channels.concat(channels)];
                    this.spinnerService.hide();
                    console.log(this.channels);
                },
                error => {
                    console.log(error);
                    this.notifications.showErrorMessage(error);
                    this.spinnerService.hide();
                },
            );
    }

    deleteChannel(channelId: number) {
        console.log(channelId);
    }
}
