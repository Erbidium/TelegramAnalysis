import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { StatisticsService } from "@core/services/statistics.service";
import { BaseComponent } from "@core/base/base.component";
import { ParsingStatistics } from "@core/models/parsing-statistics";
import { ChannelStatistics } from "@core/models/channel-statistics";

@Component({
  selector: 'app-statistics-page',
  templateUrl: './statistics-page.component.html',
  styleUrls: ['./statistics-page.component.sass']
})
export class StatisticsPageComponent extends BaseComponent implements OnInit {

    public parsingStatistics: ChannelStatistics[] = [];

  constructor(private statisticsService: StatisticsService, private changeDetectorRef: ChangeDetectorRef) {
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
                error => console.log(error) //this.notifications.showErrorMessage(error),
            );
    }

}
