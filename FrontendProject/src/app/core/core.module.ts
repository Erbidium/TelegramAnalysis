import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BaseComponent } from '@core/base/base.component';
import { SharedModule } from '@shared/shared.module';

@NgModule({
    declarations: [BaseComponent],
    imports: [
        CommonModule, HttpClientModule, SharedModule,
    ],
})
export class CoreModule { }
