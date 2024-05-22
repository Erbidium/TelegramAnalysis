import { Component, OnInit } from '@angular/core';
import { StatisticsService } from "@core/services/statistics.service";
import { BaseComponent } from "@core/base/base.component";
import { ChannelStatistics } from "@core/models/channel-statistics";
import { NotificationService } from "@core/services/notification.service";

@Component({
  selector: 'app-statistics-page',
  templateUrl: './statistics-page.component.html',
  styleUrls: ['./statistics-page.component.sass']
})
export class StatisticsPageComponent extends BaseComponent implements OnInit {

    public parsingStatistics: ChannelStatistics[] = [];

  constructor(private statisticsService: StatisticsService, private notifications: NotificationService) {
      super();
  }

    ngOnInit(): void {
        this.loadStatistics();
    }

    private loadStatistics() {
        this.statisticsService.getParsingStatistics()
            .pipe(this.untilThis)
            .subscribe(
                parsingStatistics => {
                    this.parsingStatistics = [... this.parsingStatistics.concat(parsingStatistics.channelsStatistics)];
                    //this.spinnerService.hide();
                    console.log(parsingStatistics.channelsStatistics)
                    console.log(this.parsingStatistics);
                },
                error => this.notifications.showErrorMessage(error),
            );
    }
}
