import { Injectable } from '@angular/core';
import { SpreadGraphItem } from '@core/models/spread-graph-item';
import { HttpInternalService } from '@core/services/http-internal.service';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';

@Injectable({
    providedIn: 'root',
})
export class DistributionAnalysisService {
    // eslint-disable-next-line no-empty-function
    constructor(private httpService: HttpInternalService) {}

    public getPostDistributionGraph(postText: string): Observable<SpreadGraphItem[]> {
        return this.httpService.postRequest<SpreadGraphItem[]>(`${environment.analysisUrl}/get_data`, {
            text: postText,
        });
    }
}
