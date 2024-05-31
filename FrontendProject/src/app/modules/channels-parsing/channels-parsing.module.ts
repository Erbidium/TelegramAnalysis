import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ChannelsParsingRoutingModule } from '@modules/channels-parsing/channels-parsing-routing.module';
import { MaterialModule } from '@shared/material/material.module';
import { SharedModule } from '@shared/shared.module';
import { ChannelsParsingPageComponent } from './channels-parsing-page/channels-parsing-page.component';
import { MatDatepickerModule } from "@angular/material/datepicker";
import { ReactiveFormsModule } from "@angular/forms";
import { MatNativeDateModule, NativeDateAdapter } from '@angular/material/core';

@NgModule({
    declarations: [
        ChannelsParsingPageComponent,
    ],
    imports: [
        CommonModule,
        ChannelsParsingRoutingModule,
        SharedModule,
        MaterialModule,
        ReactiveFormsModule
    ],
    providers: [
        MatDatepickerModule,
    ],
})
export class ChannelsParsingModule { }
