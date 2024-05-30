import { Injectable } from '@angular/core';
import { Channel } from '@core/models/channel';
import { HttpInternalService } from '@core/services/http-internal.service';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class ChannelService {
    public routePrefix = '/Channels';

    // eslint-disable-next-line no-empty-function
    constructor(private httpService: HttpInternalService) {}

    public getChannelsToParse(): Observable<Channel[]> {
        return this.httpService.getRequest<Channel[]>(`${this.routePrefix}/channels-to-parse`);
    }

    public saveChannel(channelLink: string) {
        return this.httpService.postRequest(`${this.routePrefix}/save-channel`, {
            channelLink
        });
    }

    public deleteChannel(id: number) {
        return this.httpService.deleteRequest(`${this.routePrefix}/delete-channel/${id}`);
    }
}
