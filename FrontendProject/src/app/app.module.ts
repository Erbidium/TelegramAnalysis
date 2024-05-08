import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppComponent } from './app.component';
import { RouterOutlet } from "@angular/router";
import { AppRoutingModule } from "./app-routing.module";
import { SharedModule } from "@shared/shared.module";
import { CoreModule } from "@core/core.module";

@NgModule({
    declarations: [AppComponent],
    imports: [BrowserModule, BrowserAnimationsModule, RouterOutlet, AppRoutingModule, SharedModule, CoreModule],
    providers: [],
    bootstrap: [AppComponent],
})
export class AppModule {}
