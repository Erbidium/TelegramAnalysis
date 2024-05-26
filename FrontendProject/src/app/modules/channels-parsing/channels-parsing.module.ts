import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ChannelsParsingRoutingModule } from '@modules/channels-parsing/channels-parsing-routing.module';
import { MaterialModule } from '@shared/material/material.module';
import { SharedModule } from '@shared/shared.module';

import { ChannelsParsingPageComponent } from './channels-parsing-page/channels-parsing-page.component';

@NgModule({
    declarations: [
        ChannelsParsingPageComponent,
    ],
    imports: [
        CommonModule, ChannelsParsingRoutingModule, SharedModule, MaterialModule,
    ],
})
export class ChannelsParsingModule { }
