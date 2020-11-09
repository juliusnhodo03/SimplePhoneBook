
export interface IPhoneBook {
  phoneBookId: number;
  name: string;
  entries: IEntry[];
}


export interface IEntry {
  entryId: number;
  phoneBookId: number;
  name: string;
  phoneNumber: string;
}

