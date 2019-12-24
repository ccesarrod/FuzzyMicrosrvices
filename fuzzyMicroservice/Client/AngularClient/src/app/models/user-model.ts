export interface User {
    email: string;
    userName: string;
    password?: string;
    firstName?: string;
    lastName?: string;
    token?: string;
    cart?:any;
  }