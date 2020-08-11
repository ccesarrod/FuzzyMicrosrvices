import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-order-details',
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.scss']
})
export class OrderDetailsComponent implements OnInit {
  id: any;
  details:any;
  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    debugger
    this.details= this.route.snapshot.data.details;
  }

}
