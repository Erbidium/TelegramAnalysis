import { Location } from '@angular/common';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { BaseComponent } from '@core/base/base.component';

@Component({
    selector: 'app-back-button',
    templateUrl: './back-button.component.html',
    styleUrls: ['./back-button.component.sass'],
})
export class BackButtonComponent extends BaseComponent {
    constructor(
        private router: Router,
        private location: Location,
    ) {
        super();
    }

    click() {
        this.location.back();
    }
}
