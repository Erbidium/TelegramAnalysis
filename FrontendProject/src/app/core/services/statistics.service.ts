import { Injectable } from '@angular/core';
import { HttpInternalService } from '@core/services/http-internal.service';
import { ParsingStatistics } from "@core/models/parsing-statistics";
import { Observable } from "rxjs";
import { ChannelStatistics } from "@core/models/channel-statistics";

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
}
