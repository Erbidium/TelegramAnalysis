import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StatisticsPageComponent } from '@modules/statistics/statistics-page/statistics-page.component';

const routes: Routes = [
    {
        path: '',
        component: StatisticsPageComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StatisticsRoutingModule {}
