import { Injectable } from '@angular/core';
import { HttpInternalService } from '@core/services/http-internal.service';

@Injectable({
    providedIn: 'root',
})
export class ParsingService {
    public routePrefix = '/Parsing';

    // eslint-disable-next-line no-empty-function
    constructor(private httpService: HttpInternalService) {}

    public parseChannels() {
        return this.httpService.postRequest(`${this.routePrefix}`, {});
    }
}
