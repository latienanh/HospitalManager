import { IFormFile } from "@proxy/microsoft/asp-net-core/http";

export function createIFormFile(file: File): IFormFile {
    return {
      contentType: file.type,
      contentDisposition: `attachment; filename=${file.name}`,
      headers: {},
      length: file.size,
      name: 'file',
      fileName: file.name
    };
  }