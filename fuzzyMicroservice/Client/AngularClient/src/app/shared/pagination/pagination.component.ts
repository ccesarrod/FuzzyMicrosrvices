import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.sass']
})
export class PaginationComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

  @Input() items:[];
  page = 1;
  pageSize = 4;
  collectionSize = this.items.length;

  // get countries(): any[] {
  //   return this.items
  //     .map((country, i) => ({id: i + 1, ...country}))
  //     .slice((this.page - 1) * this.pageSize, (this.page - 1) * this.pageSize + this.pageSize);
  // }

}
