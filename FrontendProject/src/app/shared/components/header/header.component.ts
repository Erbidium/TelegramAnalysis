import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { BaseComponent } from '@core/base/base.component';
import { headerNavLinks } from '@core/helpers/header-helpers';

@Component({
    selector: 'app-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.sass'],
})
export class HeaderComponent extends BaseComponent {
    @Input() navLinks = headerNavLinks;

    @Input() title = 'Telegram distribution analysis';

    constructor(
        private router: Router,
    ) {
        super();
    }
}
