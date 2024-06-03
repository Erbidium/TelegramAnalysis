import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@core/base/base.component';
import { ChannelStatistics } from '@core/models/channel-statistics';
import { NotificationService } from '@core/services/notification.service';
import { SpinnerOverlayService } from '@core/services/spinner-overlay.service';
import { StatisticsService } from '@core/services/statistics.service';
import { Channel } from "@core/models/channel";
import { Router } from "@angular/router";
import { Post } from "@core/models/post";

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
        private router: Router
    ) {
        super();
    }

    ngOnInit(): void {
        this.loadStatistics();
    }

    toggleChannelMessages(channel: Channel) {
        channel.showPosts = !channel.showPosts;
    }

    private loadStatistics() {
        this.spinnerService.show();

        this.statisticsService.getParsingStatistics()
            .pipe(this.untilThis)
            .subscribe(
                parsingStatistics => {
                    this.parsingStatistics = [...this.parsingStatistics.concat(parsingStatistics.channelsStatistics)];
                    this.parsingStatistics.forEach(c => c.channel.showPosts = false)
                    this.spinnerService.hide();
                },
                error => {
                    this.notifications.showErrorMessage('Трапилася помилка');
                    console.log(error);
                    this.spinnerService.hide();
                },
            );
    }

    analyzePostDistribution(post: Post) {
        this.router.navigateByUrl(`/distribution-analysis/${post.text}`);
    }
}
