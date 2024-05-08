import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseComponent } from "@core/base/base.component";
import { HttpClientModule } from "@angular/common/http";
import { SharedModule } from "@shared/shared.module";



@NgModule({
  declarations: [BaseComponent],
  imports: [
    CommonModule, HttpClientModule, SharedModule
  ]
})
export class CoreModule { }
