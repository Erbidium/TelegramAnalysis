import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {
    ChannelsParsingPageComponent,
} from '@modules/channels-parsing/channels-parsing-page/channels-parsing-page.component';

const routes: Routes = [
    {
        path: '',
        component: ChannelsParsingPageComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ChannelsParsingRoutingModule {}
