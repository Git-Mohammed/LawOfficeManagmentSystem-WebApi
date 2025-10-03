export interface ErrorResponse {
    type: string;
    title: string;
    status: number;
    errors: Error[];
    traceId:string;
}

interface Error {
    key:string;
    value:string;
}