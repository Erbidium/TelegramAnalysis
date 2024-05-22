import { Component, OnInit } from '@angular/core';
import { ChannelStatistics } from "@core/models/channel-statistics";
import { Channel } from "@core/models/channel";
import { BaseComponent } from "@core/base/base.component";
import { StatisticsService } from "@core/services/statistics.service";

@Component({
  selector: 'app-channels-parsing-page',
  templateUrl: './channels-parsing-page.component.html',
  styleUrls: ['./channels-parsing-page.component.sass']
})
export class ChannelsParsingPageComponent extends BaseComponent implements OnInit {

    public channels: Channel[] = [];

    constructor(private statisticsService: StatisticsService) {
        super();
    }

    ngOnInit(): void {
        this.loadChannels();
    }

    private loadChannels() {
        this.statisticsService.getChannelsToParse()
            .pipe(this.untilThis)
            .subscribe(
                channels => {
                    this.channels = [... this.channels.concat(channels)];
                    //this.spinnerService.hide();
                    console.log(this.channels);
                },
                error => console.log(error) //this.notifications.showErrorMessage(error),
            );
    }

    deleteChannel(channelId: number)
    {
    
    }
}
