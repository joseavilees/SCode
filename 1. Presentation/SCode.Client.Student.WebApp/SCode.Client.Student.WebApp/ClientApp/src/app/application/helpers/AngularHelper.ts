import { isDevMode } from "@angular/core";


export function delay(ms: number) {
    return new Promise( resolve => setTimeout(resolve, ms) );
}
