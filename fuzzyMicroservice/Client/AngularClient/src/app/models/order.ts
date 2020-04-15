export interface IOrder {
    ShipCity: string;
    ShipAddress: string;
    ShipRegion: string;
    ShipCountry : number;
    ShipPostalCode : string;
    cardnumber: string;
    cardexpiration?: Date;
    expiration?: string;
    cardsecuritynumber: string;
    cardholdername: string;
    cardtypeid: number;
    buyer?: string;
    ordernumber?: string;
    total: number;
   // orderItems: IOrderItem[];
}