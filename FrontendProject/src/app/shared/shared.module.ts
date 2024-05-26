import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterModule } from '@angular/router';

import { BackButtonComponent } from './components/back-button/back-button.component';
import { HeaderComponent } from './components/header/header.component';
import { SpinnerComponent } from './components/spinner/spinner.component';
import { SpinnerOverlayComponent } from './components/spinner-overlay/spinner-overlay.component';
import { MaterialModule } from './material/material.module';

@NgModule({
    declarations: [
        BackButtonComponent,
        HeaderComponent,
        SpinnerComponent,
        SpinnerOverlayComponent,
    ],
    imports: [
        CommonModule,
        MaterialModule,
        RouterModule,
        MatIconModule,
        MatTabsModule,
        MatToolbarModule,
    ],
    exports: [
        RouterModule,
        BackButtonComponent,
        HeaderComponent,
    ],
})
export class SharedModule { }
