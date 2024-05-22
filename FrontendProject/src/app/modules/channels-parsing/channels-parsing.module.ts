import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChannelsParsingPageComponent } from './channels-parsing-page/channels-parsing-page.component';
import { ChannelsParsingRoutingModule } from "@modules/channels-parsing/channels-parsing-routing.module";
import { SharedModule } from "@shared/shared.module";
import { MatButtonModule } from "@angular/material/button";
import { MatCardModule } from "@angular/material/card";
import { MatIconModule } from "@angular/material/icon";
import { MaterialModule } from "@shared/material/material.module";



@NgModule({
  declarations: [
    ChannelsParsingPageComponent
  ],
    imports: [
        CommonModule, ChannelsParsingRoutingModule, SharedModule, MaterialModule
    ]
})
export class ChannelsParsingModule { }
