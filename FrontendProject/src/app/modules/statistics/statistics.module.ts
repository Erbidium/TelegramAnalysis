import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { StatisticsRoutingModule } from '@modules/statistics/statistics-routing.module';
import { MaterialModule } from '@shared/material/material.module';
import { SharedModule } from '@shared/shared.module';

import { StatisticsPageComponent } from './statistics-page/statistics-page.component';

@NgModule({
    declarations: [
        StatisticsPageComponent,
    ],
    imports: [
        CommonModule, StatisticsRoutingModule, MaterialModule, SharedModule,
    ],
})
export class StatisticsModule { }
