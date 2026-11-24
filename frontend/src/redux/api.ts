import { createApi, fetchBaseQuery, retry } from '@reduxjs/toolkit/query/react';

import { apiUrl } from '../fetchHelper';
import { VideoMetadata } from '../models/VideoMetadata';

// Enhanced base query with error handling
const baseQuery = fetchBaseQuery({ 
  baseUrl: apiUrl,
  timeout: 10000, // 10 second timeout
});

const baseQueryWithRetry = retry(baseQuery, {
  maxRetries: 2,
});

const api = createApi({
  reducerPath: 'api',
  baseQuery: baseQueryWithRetry,
  tagTypes: ['Videos', 'Thumbnails', 'Tags', 'TagVideos', 'Directories', 'ContactSheets'],
  endpoints: (builder) => ({
    // Health check endpoint to test API availability
    healthCheck: builder.query<{ status: string }, void>({
      query: () => 'health',
      // Don't cache health checks
      keepUnusedDataFor: 0,
    }),
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

export const { useHealthCheckQuery, useLazyHealthCheckQuery } = api;
