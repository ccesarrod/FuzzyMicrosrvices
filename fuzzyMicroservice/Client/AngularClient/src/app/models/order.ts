import { ICartItem } from './cartItem-model';
import { DecimalPipe } from '@angular/common';

export interface IOrder {
    Id?:number,
    ShipCity: string;
    ShipAddress: string;
    ShipRegion: string;
    ShipCountry : number;
    ShipPostalCode : string;
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




