import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from "./material/material.module";
import { RouterModule } from "@angular/router";
import { BackButtonComponent } from './components/back-button/back-button.component';
import { MatIconModule } from "@angular/material/icon";
import { HeaderComponent } from './components/header/header.component';
import { MatTabsModule } from "@angular/material/tabs";
import { MatToolbarModule } from "@angular/material/toolbar";
import { SpinnerComponent } from './components/spinner/spinner.component';
import { SpinnerOverlayComponent } from './components/spinner-overlay/spinner-overlay.component';



@NgModule({
    declarations: [
    BackButtonComponent,
    HeaderComponent,
    SpinnerComponent,
    SpinnerOverlayComponent
  ],
    imports: [
        CommonModule,
        MaterialModule,
        RouterModule,
        MatIconModule,
        MatTabsModule,
        MatToolbarModule
    ],
    exports: [
      RouterModule,
        BackButtonComponent,
        HeaderComponent
    ]
})
export class SharedModule { }
