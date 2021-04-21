import { Component, OnInit, TemplateRef } from '@angular/core';
import { AppToastService } from '@services/app-toast.service';

@Component({
  selector: 'app-app-toast',
  templateUrl: './app-toast.component.html',
  styleUrls: ['./app-toast.component.scss'],

})
export class AppToastComponent implements OnInit {

  constructor(public toastService: AppToastService) { }

  ngOnInit() {
  }

  showStandard(msg ) {
    this.toastService.show(msg);
  }

  showSuccess(msg) {
    this.toastService.show(msg, { classname: 'bg-success text-light', delay: 10000 });
  }

  showDanger(dangerTpl) {
    this.toastService.show(dangerTpl, { classname: 'bg-danger text-light', delay: 15000 });
  }

  isTemplate(toast) { return toast.textOrTpl instanceof TemplateRef; }

}
