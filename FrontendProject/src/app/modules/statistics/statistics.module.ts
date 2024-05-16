import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StatisticsPageComponent } from './statistics-page/statistics-page.component';
import { StatisticsRoutingModule } from "@modules/statistics/statistics-routing.module";
import { MaterialModule } from "@shared/material/material.module";
import { MatCardModule } from "@angular/material/card";



@NgModule({
  declarations: [
    StatisticsPageComponent
  ],
    imports: [
        CommonModule, StatisticsRoutingModule, MaterialModule, MatCardModule
    ]
})
export class StatisticsModule { }