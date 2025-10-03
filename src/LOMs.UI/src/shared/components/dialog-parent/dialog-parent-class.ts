import { MatDialogRef } from "@angular/material/dialog";

export class DialogParentClass<DType> {
 
    constructor(public dialogRef:MatDialogRef<DType>) {
        
    }
}
