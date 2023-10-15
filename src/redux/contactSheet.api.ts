import {
  EntityState,
  createEntityAdapter,
  createSelector
} from '@reduxjs/toolkit';
import { Tag } from '../models/Tag';
import api from './api';
import { RootState } from './configureStore';
import { ContactSheet } from '../models/ContactSheet';

// const entityAdapter = createEntityAdapter<ContactSheet>({
//   selectId: (t) => t.id,
//   sortComparer: (a, b) => (a.videoId < b.videoId ? -1 : 1)
// });

// const initialState = entityAdapter.getInitialState();

const contactSheetApi = api.injectEndpoints({
  endpoints: (builder) => ({
    getContactSheetForVideo: builder.query<ContactSheet, number>({
      query: (id) => ({ url: 'contactSheet', params: { videoId: id } }),
      providesTags: (result, error, arg) => {
        return [{ type: 'ContactSheets', id: arg }];
      },
      // transformResponse: (response: ContactSheet) =>
      //   entityAdapter.setOne(initialState, response)
    }),
    // addTag: builder.mutation<EntityState<Tag>, Tag>({
    //   query: (tag) => ({
    //     url: 'tags',
    //     method: 'POST',
    //     body: tag
    //   }),
    //   invalidatesTags: (result) => {
    //     return ['Tags'];
    //   },
    //   transformResponse: (response: Tag) =>
    //     entityAdapter.setOne(initialState, response)
    // })
  })
});

// const selector = contactSheetApi.endpoints.getTags.select();
// const tagsSelector = createSelector(selector, (tags) => tags.data);
// export const {
//   selectAll: getContactSheetList,
//   selectById: getContactSheet,
//   selectEntities: getContactSheetDictionary
// } = entityAdapter.getSelectors<RootState>(
//   (state) => tagsSelector(state) ?? initialState
// );

export const { useGetContactSheetForVideoQuery, useLazyGetContactSheetForVideoQuery } = contactSheetApi;
