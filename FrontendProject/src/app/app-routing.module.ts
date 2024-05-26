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
    {
        path: 'distribution-analysis',
        loadChildren: () =>
            import('./modules/distribution-analysis/distribution-analysis.module').then(
                (m) => m.DistributionAnalysisModule,
            ),
    },
    {
        path: 'channels-parsing',
        loadChildren: () =>
            import('./modules/channels-parsing/channels-parsing.module').then((m) => m.ChannelsParsingModule),
    },
    { path: '**', redirectTo: '', pathMatch: 'full' },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
})
export class AppRoutingModule {}
