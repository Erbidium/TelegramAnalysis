import { Injectable } from '@angular/core';
import { Channel } from '@core/models/channel';
import { ParsingStatistics } from '@core/models/parsing-statistics';
import { SpreadGraphItem } from '@core/models/spread-graph-item';
import { HttpInternalService } from '@core/services/http-internal.service';
import { Observable } from 'rxjs';

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
