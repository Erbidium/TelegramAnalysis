import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from "@shared/material/material.module";
import { MatCardModule } from "@angular/material/card";
import {
    DistributionAnalysisPageComponent
} from "@modules/distribution-analysis/distribution-analysis-page/distribution-analysis-page.component";
import { DistributionAnalysisRoutingModule } from "@modules/distribution-analysis/distribution-analysis-routing.module";
import { NgxEchartsModule } from "ngx-echarts";
import { SharedModule } from "@shared/shared.module";
import { MatInputModule } from "@angular/material/input";
import { ReactiveFormsModule } from "@angular/forms";
import { MatButtonModule } from "@angular/material/button";



@NgModule({
    declarations: [
        DistributionAnalysisPageComponent
    ],
    imports: [
        CommonModule, DistributionAnalysisRoutingModule, MaterialModule, MatCardModule,
        NgxEchartsModule.forRoot({
            /**
             * This will import all modules from echarts.
             * If you only need custom modules,
             * please refer to [Custom Build] section.
             */
            echarts: () => import('echarts'), // or import('./path-to-my-custom-echarts')
        }), SharedModule, MatInputModule, ReactiveFormsModule, MatButtonModule
    ]
})
export class DistributionAnalysisModule { }
