import { inject, Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({ providedIn: 'root' })
export class NotificationService {
    constructor(private snackBar: MatSnackBar) {
        this.snackBar = inject(MatSnackBar);
    }

    showErrorMessage(error: string, action: string = '') {
        this.snackBar.open(error, action, { duration: 2500, panelClass: 'error-snack-bar' });
    }

    showInfoMessage(message: string, action: string = '') {
        this.snackBar.open(message, action, { duration: 2500, panelClass: 'info-snack-bar' });
    }

    showWarningMessage(message: string, action: string = '') {
        this.snackBar.open(message, action, { duration: 2500, panelClass: 'warning-snack-bar' });
    }

    showSuccessMessage(message: string, action: string = '') {
        this.snackBar.open(message, action, { duration: 2500, panelClass: 'success-snack-bar' });
    }
}
