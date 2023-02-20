import { Tag } from '../models/Tag';
import api from './api';

const tagApi = api.injectEndpoints({
  endpoints: (builder) => ({
    getTags: builder.query<Tag[], void>({
      query: () => 'tags',
      providesTags: (result) => {
        return ['Tags'];
      }
    })
  })
});

export const { useGetTagsQuery } = tagApi;
