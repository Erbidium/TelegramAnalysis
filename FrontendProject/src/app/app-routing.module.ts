import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
    {
        path: '',
        redirectTo: 'statistics',
        pathMatch: 'full',
    },
    {
        path: 'statistics',
        loadChildren: () => import('./modules/statistics/statistics.module').then((m) => m.StatisticsModule),
    },
    { path: '**', redirectTo: '', pathMatch: 'full' },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
})
export class AppRoutingModule {}
