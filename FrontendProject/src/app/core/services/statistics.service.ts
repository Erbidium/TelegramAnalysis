import { Injectable } from '@angular/core';
import { HttpInternalService } from '@core/services/http-internal.service';
import { ParsingStatistics } from "@core/models/parsing-statistics";
import { Observable } from "rxjs";
import { ChannelStatistics } from "@core/models/channel-statistics";
import { SpreadGraphItem } from "@core/models/spread-graph-item";

@Injectable({
    providedIn: 'root',
})
export class StatisticsService {
    public routePrefix = '/Parsing';

    // eslint-disable-next-line no-empty-function
    constructor(private httpService: HttpInternalService) {}

    public getParsingStatistics(): Observable<ParsingStatistics> {
        return this.httpService.getRequest<ParsingStatistics>(`${this.routePrefix}/parsing-statistics`);
    }

    public getGraph(postText: string): Observable<SpreadGraphItem[]> {
        return this.httpService.postRequest<SpreadGraphItem[]>('http://127.0.0.1:5000/get_data', {
            text: postText
        });
    }
}
