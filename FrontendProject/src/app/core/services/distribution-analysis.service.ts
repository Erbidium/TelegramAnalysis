import { Injectable } from '@angular/core';
import { DistributionGraphNode } from '@core/models/distribution-graph-node';
import { HttpInternalService } from '@core/services/http-internal.service';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';

@Injectable({
    providedIn: 'root',
})
export class DistributionAnalysisService {
    // eslint-disable-next-line no-empty-function
    constructor(private httpService: HttpInternalService) {}

    public getPostDistributionGraph(postText: string): Observable<DistributionGraphNode[]> {
        return this.httpService.postRequest<DistributionGraphNode[]>(`${environment.analysisUrl}/get_data`, {
            text: postText,
        });
    }
}
