interface ValidationError {
  message: string;
  members: string[];
}

export interface ErrorResponse {
  headers: {
    normalizedNames: Record<string, string>;
    lazyUpdate: any;
  };
  status: number;
  statusText: string;
  url: string;
  ok: boolean;
  name: string;
  message: string;
  error: {
    error: {
      code: string | null;
      message: string;
      details: string | null;
      data: {
        message:string
      };
      validationErrors: ValidationError[] | null;
    };
  };
}
