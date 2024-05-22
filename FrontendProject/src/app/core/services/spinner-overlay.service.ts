import { Overlay, OverlayRef } from '@angular/cdk/overlay';
import { ComponentPortal } from '@angular/cdk/portal';
import { Injectable } from '@angular/core';
import { SpinnerOverlayComponent } from '@shared/components/spinner-overlay/spinner-overlay.component';

@Injectable({
    providedIn: 'root',
})
export class SpinnerOverlayService {
    private overlayRef: OverlayRef | null = null;

    // eslint-disable-next-line no-empty-function
    constructor(private overlay: Overlay) { }

    show() {
        if (!this.overlayRef) {
            this.overlayRef = this.overlay.create();
        }

        const spinnerOverlayPortal = new ComponentPortal(SpinnerOverlayComponent);

        this.overlayRef.attach(spinnerOverlayPortal);
    }

    hide() {
        if (this.overlayRef) {
            this.overlayRef.detach();
        }
    }
}
