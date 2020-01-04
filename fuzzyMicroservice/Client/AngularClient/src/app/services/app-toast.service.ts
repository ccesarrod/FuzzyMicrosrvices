import { Injectable, TemplateRef } from '@angular/core';
import { Subject, Observable } from 'rxjs';
import { NavigationStart, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AppToastService {




  constructor() {
    
  }
  toasts: any[] = [];



  show(textOrTpl: string | TemplateRef<any>, options: any = {}) {
    this.toasts.push({ textOrTpl, ...options });
  }

  remove(toast) {
    this.toasts = this.toasts.filter(t => t !== toast);
  }
  
}
