import { ICartItem } from './cartItem-model';
import { DecimalPipe } from '@angular/common';

export interface IOrder {
    orderID?:number,
    shipCity: string;
    shipAddress: string;
    shipRegion: string;
    shipCountry : number;
    shipPostalCode : string;
    orderDate?:string; 
    requiredDate?:Date;
    shippedDate?:Date;
   /*  cardnumber: string;
    cardexpiration?: Date;
    expiration?: string;
    cardsecuritynumber: string;
    cardholdername: string;
    cardtypeid: number;
    buyer?: string; */
   // ordernumber?: string;
   // total: number;  
   order_Detail:ICartItem[];
}




