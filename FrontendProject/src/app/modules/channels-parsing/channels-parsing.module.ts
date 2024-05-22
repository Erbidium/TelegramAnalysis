import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChannelsParsingPageComponent } from './channels-parsing-page/channels-parsing-page.component';
import { ChannelsParsingRoutingModule } from "@modules/channels-parsing/channels-parsing-routing.module";
import { SharedModule } from "@shared/shared.module";
import { MatButtonModule } from "@angular/material/button";
import { MatCardModule } from "@angular/material/card";
import { MatIconModule } from "@angular/material/icon";



@NgModule({
  declarations: [
    ChannelsParsingPageComponent
  ],
    imports: [
        CommonModule, ChannelsParsingRoutingModule, SharedModule, MatButtonModule, MatCardModule, MatIconModule
    ]
})
export class ChannelsParsingModule { }
