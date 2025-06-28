import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

import { apiUrl } from '../fetchHelper';
import { VideoMetadata } from '../models/VideoMetadata';

const api = createApi({
  reducerPath: 'api',
  baseQuery: fetchBaseQuery({ baseUrl: apiUrl }),
  tagTypes: ['Videos', 'Thumbnails', 'Tags', 'TagVideos', 'Directories', 'ContactSheets'],
  endpoints: (builder) => ({
    // getMetadata: builder.query<VideoMetadata[], void>({
    //   query: () => 'metadata'
    // }),
    // getMetadataForVideo: builder.query<VideoMetadata, string>({
    //   query: (filePath: string) =>
    //     'metadata?' + new URLSearchParams({ filePath })
    // })
  })
});

export default api;

export const {} = api;
