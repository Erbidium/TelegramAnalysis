import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@core/base/base.component';
import { ChannelStatistics } from '@core/models/channel-statistics';
import { NotificationService } from '@core/services/notification.service';
import { SpinnerOverlayService } from '@core/services/spinner-overlay.service';
import { StatisticsService } from '@core/services/statistics.service';

@Component({
    selector: 'app-statistics-page',
    templateUrl: './statistics-page.component.html',
    styleUrls: ['./statistics-page.component.sass'],
})
export class StatisticsPageComponent extends BaseComponent implements OnInit {
    public parsingStatistics: ChannelStatistics[] = [];

    constructor(
        private statisticsService: StatisticsService,
        private notifications: NotificationService,
        private spinnerService: SpinnerOverlayService,
    ) {
        super();
    }

    ngOnInit(): void {
        this.loadStatistics();
    }

    private loadStatistics() {
        this.spinnerService.show();

        this.statisticsService.getParsingStatistics()
            .pipe(this.untilThis)
            .subscribe(
                parsingStatistics => {
                    this.parsingStatistics = [...this.parsingStatistics.concat(parsingStatistics.channelsStatistics)];
                    this.spinnerService.hide();
                },
                error => {
                    this.notifications.showErrorMessage('Трапилася помилка');
                    console.log(error);
                },
            );
    }
}
