import { ICartItem } from './cartItem-model';
import { DecimalPipe } from '@angular/common';

export interface IOrder {
    OrderID?:number,
    ShipCity: string;
    ShipAddress: string;
    ShipRegion: string;
    ShipCountry : number;
    ShipPostalCode : string;
    OrderDate?:Date; 
    RequiredDate?:Date;
    ShippedDate?:Date;
   /*  cardnumber: string;
    cardexpiration?: Date;
    expiration?: string;
    cardsecuritynumber: string;
    cardholdername: string;
    cardtypeid: number;
    buyer?: string; */
   // ordernumber?: string;
   // total: number;  
   Order_Detail:ICartItem[];
}




