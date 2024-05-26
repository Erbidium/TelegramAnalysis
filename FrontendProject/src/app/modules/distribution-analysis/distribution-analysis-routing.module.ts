import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {
    DistributionAnalysisPageComponent,
} from '@modules/distribution-analysis/distribution-analysis-page/distribution-analysis-page.component';

const routes: Routes = [
    {
        path: '',
        component: DistributionAnalysisPageComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class DistributionAnalysisRoutingModule {}
