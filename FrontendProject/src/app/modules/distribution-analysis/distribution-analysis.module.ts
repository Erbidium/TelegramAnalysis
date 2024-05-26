import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import {
    DistributionAnalysisPageComponent,
} from '@modules/distribution-analysis/distribution-analysis-page/distribution-analysis-page.component';
import { DistributionAnalysisRoutingModule } from '@modules/distribution-analysis/distribution-analysis-routing.module';
import { MaterialModule } from '@shared/material/material.module';
import { SharedModule } from '@shared/shared.module';
import { NgxEchartsModule } from 'ngx-echarts';

@NgModule({
    declarations: [
        DistributionAnalysisPageComponent,
    ],
    imports: [
        CommonModule, DistributionAnalysisRoutingModule, MaterialModule,
        NgxEchartsModule.forRoot({
            echarts: () => import('echarts'),
        }), SharedModule, ReactiveFormsModule,
    ],
})
export class DistributionAnalysisModule { }
